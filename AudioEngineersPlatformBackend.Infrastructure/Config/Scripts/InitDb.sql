CREATE USER "audio-engineers-platform-backend-user" WITH PASSWORD 'hardPassword1234!';

CREATE DATABASE "audio-engineers-platform-db";

GRANT ALL PRIVILEGES ON DATABASE "audio-engineers-platform-db" TO "audio-engineers-platform-backend-user";