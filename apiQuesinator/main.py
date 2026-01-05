from fastapi import FastAPI, HTTPException, Header
from datetime import datetime, timedelta
import sqlite3
import secrets
import hashlib

app = FastAPI()

DB = "game.db"

# ---------- Helpers ----------

def hash_token(token: str) -> str:
    return hashlib.sha256(token.encode()).hexdigest()

def generate_token() -> str:
    return secrets.token_urlsafe(32)

def get_db():
    return sqlite3.connect(DB)

# ---------- Create Session ----------

@app.post("/session/create")
def create_session(user_id: int):
    db = get_db()
    cur = db.cursor()

    # Check user
    cur.execute("SELECT id FROM users WHERE id = ?", (user_id,))
    if not cur.fetchone():
        raise HTTPException(404, "User not found")

    token = generate_token()
    token_hash = hash_token(token)
    expires = datetime.utcnow() + timedelta(hours=1)

    cur.execute("""
        INSERT INTO sessions (user_id, token_hash, expires_at)
        VALUES (?, ?, ?)
    """, (user_id, token_hash, expires))

    db.commit()
    db.close()

    return {
        "sessionToken": token,
        "expiresAt": expires.isoformat()
    }

@app.get("/game/validate")
def validate_session(authorization: str = Header(None)):
    if not authorization:
        raise HTTPException(401, "Missing token")

    token = authorization.replace("Bearer ", "")
    token_hash = hash_token(token)

    db = get_db()
    cur = db.cursor()

    cur.execute("""
        SELECT user_id FROM sessions
        WHERE token_hash = ?
        AND expires_at > ?
    """, (token_hash, datetime.utcnow()))

    row = cur.fetchone()
    db.close()

    if not row:
        raise HTTPException(401, "Invalid or expired session")

    return {
        "userId": row[0],
        "status": "valid"
    }
