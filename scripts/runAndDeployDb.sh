#!/bin/bash


set +e

cd ./database/Local

sudo docker build -t safekeydb .

sudo docker stop safekey-database

sudo docker rm safekey-database

sudo docker run -d --name safekey-database -p 127.0.0.1:5432:5432 safekeydb

sudo docker start safekey-database

cd ../..
