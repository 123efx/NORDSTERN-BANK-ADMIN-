from fastapi import FastAPI
from pydantic import BaseModel
import pymysql
import requests
import socket

app = FastAPI(
    title="Nordenstern Bank API",
    version="1.0.1"
)

# -----------------------------
# Datenmodell für Request
# -----------------------------

class LoginLog(BaseModel):
    username: str
    role: str

# -----------------------------
# MySQL Connection Helper
# -----------------------------

def get_connection():
    return pymysql.connect(
        host="localhost",
        user="root",
        password="efeates2008",
        database="nordstern_bank",
        charset="utf8mb4",
        cursorclass=pymysql.cursors.DictCursor,
        autocommit=True
    )

# -----------------------------
# Health Check (optional, aber gut)
# -----------------------------

@app.get("/health")
def health():
    return {"status": "ok"}

# -----------------------------
# Hilfsfunktion: lokale IP holen
# -----------------------------

def get_local_ip():
    local_ip = "unknown"
    try:
        hostname = socket.gethostname()
        for ip in socket.gethostbyname_ex(hostname)[2]:
            if ip.startswith("192.") or ip.startswith("10.") or ip.startswith("172."):
                local_ip = ip
                break
    except Exception:
        pass
    return local_ip

# -----------------------------
# Login-IP-Logging Endpoint
# -----------------------------

@app.post("/log_ip")
def log_ip(data: LoginLog):
    # IP + Location holen
    ip_address = "unknown"
    city = None
    country = None

    try:
        ip_response = requests.get("https://ipapi.co/json/", timeout=3).json()
        ip_address = ip_response.get("ip", "unknown")
        city = ip_response.get("city")
        country = ip_response.get("country_name")
    except Exception:
        pass

    # Falls keine externe IP, fallback auf lokale IP
    if ip_address in [None, "", "unknown"]:
        ip_address = get_local_ip()
        city = "local"
        country = "local"

    # In DB schreiben
    try:
        conn = get_connection()
        with conn.cursor() as cursor:
            sql = """
            INSERT INTO login_logs
            (username, role, ip_address, city, country, login_time)
            VALUES (%s, %s, %s, %s, %s, NOW())
            """
            cursor.execute(sql, (
                data.username,
                data.role,
                ip_address,
                city,
                country
            ))
        conn.close()
        return {"status": "success", "ip_logged": ip_address}
    except Exception as e:
        return {"status": "error", "message": str(e)}