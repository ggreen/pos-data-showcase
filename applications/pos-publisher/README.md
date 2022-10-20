Setup connection

```shell
export RABBIT_CLIENT_NAME="pos-publisher"
export CRYPTION_KEY=SECRET
export RABBIT_USERNAME=guest
export RABBIT_PASSWORD=....
```

{"id": "1", "name" : "from rabbit"}



# Docker


```shell
cd applications/pos-publisher
```

# Docker Build

```shell
docker build --tag pos-publisher:0.0.1-SNAPSHOT .
```

docker tag pos-publisher:0.0.1-SNAPSHOT cloudnativedata/pos-publisher:0.0.1-SNAPSHOT
docker push cloudnativedata/pos-publisher:0.0.1-SNAPSHOT