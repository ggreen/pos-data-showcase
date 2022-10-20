```shell
kubectl apply -f deployment/cloud/k8/data-services/rabbitmq/rabbitmq.yml
```


kubectl apply -f deployment/cloud/k8/data-services/postgres/postgres.yml

kubectl apply -f deployment/cloud/k8/data-services/gemfire/gemfire-redis.yml

# Get Secret 

kubectl get secret rabbitmq-default-user -o jsonpath="{.data.username}"
export ruser=`kubectl get secret rabbitmq-default-user -o jsonpath="{.data.username}"| base64 --decode`
export rpwd=`kubectl get secret rabbitmq-default-user -o jsonpath="{.data.password}"| base64 --decode`

echo ""
echo "USER:" $ruser
echo "PASWORD:" $rpwd


k port-forward service/rabbitmq 6672:5672

retail/retail


k apply -f deployment/cloud/k8/apps/pos-publisher/pos-publisher.yml