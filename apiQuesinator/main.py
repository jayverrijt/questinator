import pyodbc
import secrets
import hashlib
from datetime import datetime, timedelta, UTC

# ---------- CONFIG ----------

CONNECTION_STRING = (
    "DRIVER={ODBC Driver 18 for SQL Server};"
    "SERVER=localhost\\SQLEXPRESS;"
    "DATABASE=MyDatabase;"
    "Trusted_Connection=yes;"
    "TrustServerCertificate=yes;"
)

# ---------- HELPERS ----------

def get_db():
    return pyodbc.connect(CONNECTION_STRING)

def generate_token() -> str:
    return secrets.token_urlsafe(32)

def hash_token(token: str) -> str:
    return hashlib.sha256(token.encode()).hexdigest()

# ---------- MAIN ----------

def main():
    print("=== Game Session Token Generator ===\n")

    user_id = input("Voer UserId in (string): ").strip()
    if not user_id:
        print("❌ UserId mag niet leeg zijn")
        return

    db = get_db()
    cur = db.cursor()

    # ✅ JUISTE QUERY
    cur.execute(
        "SELECT Id FROM Users WHERE UserId = ?",
        user_id
    )

    row = cur.fetchone()
    if not row:
        print("❌ User niet gevonden in database")
        db.close()
        return

    user_pk = row[0]  # dit is de Id (INT)

    # Genereer token
    token = generate_token()
    token_hash = hash_token(token)
    expires_at = datetime.now(UTC) + timedelta(hours=1)

    db.close()

    print("\n✅ User gevonden!")
    print(f"Database Id: {user_pk}")
    print("\nSession token:")
    print(token)
    print("\nVerloopt op (UTC):")
    print(expires_at.isoformat())

if __name__ == "__main__":
    main()
