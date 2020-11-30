Run local infrastructure with `docker-compose up -d`

The postgres database details are:

- Host: localhost
- Port: 5432
- Database name: postgres
- Database user: postgres

This means you can connect to it using `psql -h localhost -p 5432 -d postgres -U postgres`
