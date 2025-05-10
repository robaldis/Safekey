create user admin with password 'adminpassword';
alter user admin with superuser;
alter user admin createdb;
alter user admin createrole;

CREATE DATABASE safekey;

GRANT ALL PRIVILEGES ON DATABASE safekey to admin;
