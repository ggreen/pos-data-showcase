```shell
export RABBIT_CLIENT_NAME="pos-publisher"
export RABBIT_USERNAME=guest
export RABBIT_PASSWORD=....
export CRYPTION_KEY=SECRET
export REDIS_CONNECTION_STRING="localhost:6379,localhost:6372,connectRetry=10"
```


# Docker Build


```shell
docker build --tag pos-consumer:0.0.1-SNAPSHOT .
```


```shell
docker run --env REDIS_CONNECTION_STRING=host.docker.internal:6379,host.docker.internal:6372,connectRetry=10 --env CRYPTION_KEY=SECRET --env RABBIT_CLIENT_NAME="pos-publisher" --env RABBIT_USERNAME=guest --env RABBIT_PASSWORD=guest --env CRYPTION_KEY=SECRET --env RABBIT_URI="amqp://guest:guest@docker-rabbitmq-cluster_default:5672/" pos-consumer:0.0.1-SNAPSHOT 
```
