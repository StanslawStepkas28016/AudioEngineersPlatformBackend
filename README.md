# Engineers Thesis s28016 - Stanisław Stepka

Repository contains files for the server application
that handles requests from the
following [client application](https://github.com/StanslawStepkas28016/audio-engineers-platform-frontend).

The thesis is written in Polish, as part of the Engineers of IT studies at PJATK.

## Installation guide

1. Build a Docker image using docker CLI.

```bash
docker build -f Dockerfile -t soundbest-api:1.1 --platform
linux/amd64,linux/arm64 .
```

2. Tag the built image.

```bash
docker tag soundbest-api:1.1 stanislawstepkas28016/soundbest-
api:1.1
```

3. Push it to DockerHub or any other remote repository.

```bash
docker login

docker push stanislawstepkas28016/soundbest-api:1.1
```

4. Build a `docker-comopose.yml` file for all related services. You must ensure that you have Aspire CLI installed, if
   not, refer to the following [docs](https://aspire.dev/get-started/install-cli/).

```bash
cd AppHost

aspire publish -o docker-compose-artifacts
```

5. Fill the `.env` that has been created using the Aspire CLI.

```bash
AWSACCESSKEY=YOUR_AWS_ACCESSKEY
AWSSECRETKEY=YOUR_AWS_SECRETKEY
DBCONNECTIONSTRING='Host=soundbest-db;Port=5432;Database=audio-engineers-
platform-db;User Id=audio-engineers-platform-backend-
user;Password=YOUR_POSTGRES_PASSWORD!'
ENV=Production
JWTSECRET=YOUR_JWT_SECRET
POSTGRESPASSWORD=YOUR_POSTGRES_PASSWORD
POSTGRESUSERNAME=postgres
SOUNDBEST_API_IMAGE=stanislawstepkas28016/soundbest-api:1.1
SOUNDBEST_API_PORT=9080
SOUNDBEST_DB_BINDMOUNT_0=scripts-soundbest/
SOUNDBEST_FRONTEND_IMAGE=stanislawstepkas28016/soundbest-frontend:1.1
```

6. Connect to your remote server and do the following:

- Create a init script for the database `nano scripts-soundbest/InitDb.sql` and fill it with the following script.

```sql
CREATE USER "audio-engineers-platform-backend-user" WITH
PASSWORD 'YOUR_POSTGRES_PASSWORD';
CREATE DATABASE "audio-engineers-platform-db";
GRANT ALL PRIVILEGES ON DATABASE "audio-engineers-platform-db"
TO "audio-engineers-platform-backend-user";
```

7. Create and `.env` using `nano .env` file and fill it with values from the `5.` point.


8. Install nginx, Docker CLI and Certbot.

```bash
sudo apt install docker-ce docker-cli containerd.io dockerx-
buildx-plugin docker-compose-plugin
docker login
apt update
apt install nginx
apt install certbot python3-certbot-nginx
```

Create SSL certificates for a domain you bought, in my case its `soundbest.pl`. Ensure creating an A type DNS record at
your domain provider.

```bash
certbot --nginx -d soundbest.pl
certbot --nginx -d www.soundbest.pl
certbot --nginx -d api.soundbest.pl
```

9. Create an nginx configuration file using `nano /etc/nginx/sites-available/my-server.conf` and fill it with the
   following. After that, restart nginx using `systemctl reload nginx.service`.

```bash
server {
    listen 443 ssl;
    server_name api.soundbest.pl;
    
    location / {
    proxy_pass http://localhost:9080;
    proxy_set_header Upgrade $http_upgrade;
    proxy_set_header Connection $http_connection;
    }
    
    ssl_certificate /etc/letsencrypt/live/api.soundbest.pl/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/api.soundbest.pl/privkey.pem;
    include /etc/letsencrypt/options-ssl-nginx.conf;
    ssl_dhparam /etc/letsencrypt/ssl-dhparams.pem;
}
server {
    listen 80;
    server_name api.soundbest.pl;
    
    if ($scheme != "https") {
    return 301 https://$host$request_uri;
    }
}
server {
    listen 443 ssl;
    server_name soundbest.pl;
    
    location / {
        proxy_pass http://localhost:5173;
    }
    
    ssl_certificate /etc/letsencrypt/live/soundbest.pl/fullchain1.pem;
    ssl_certificate_key /etc/letsencrypt/live/soundbest.pl/privkey1.pem;
    include /etc/letsencrypt/options-ssl-nginx.conf;
    ssl_dhparam /etc/letsencrypt/ssl-dhparams.pem;
}
server {
    listen 80;
    server_name soundbest.pl;
    
    if ($scheme != "https") {
    return 301 https://$host$request_uri;
    }
}
server {
    listen 443 ssl;
    server_name www.soundbest.pl;
    return 301 $scheme://soundbest.pl$request_uri;
}
```

10. Ensure you have created an image for the client application using the
    following [README](https://github.com/StanslawStepkas28016/audio-engineers-platform-frontend/blob/main/README.md)
    and run the docker-compose orchestration using `docker compose up -d`. Your app should be working fine.