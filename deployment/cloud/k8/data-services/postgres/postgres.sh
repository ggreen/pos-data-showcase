#!/bin/bash

set -x #echo on

# Set GemFire Pre-Requisite

kubectl create namespace cert-manager
helm repo add jetstack https://charts.jetstack.io

helm repo update

helm install cert-manager jetstack/cert-manager --namespace cert-manager  --version v1.0.2 --set installCRDs=true
kubectl create secret docker-registry image-pull-secret --docker-server=registry.pivotal.io --docker-username=$HARBOR_USER --docker-password=$HARBOR_PASSWORD


# Install Postgres



docker load -i ~/dataServices/postgres/postgres-for-kubernetes-v1.2.0/images/postgres-instance
docker load -i ~/dataServices/postgres/postgres-for-kubernetes-v1.2.0/images/postgres-operator
docker images "postgres-*"
export HELM_EXPERIMENTAL_OCI=1
helm registry login registry.pivotal.io --username=$HARBOR_USER --password=$HARBOR_PASSWORD

helm chart pull registry.pivotal.io/tanzu-sql-postgres/postgres-operator-chart:v1.2.0
helm chart export registry.pivotal.io/tanzu-sql-postgres/postgres-operator-chart:v1.2.0  --destination=/tmp/

kubectl create secret docker-registry regsecret \
--docker-server=https://registry.pivotal.io --docker-username=$HARBOR_USER \
--docker-password=$HARBOR_PASSWORD


kubectl create namespace sql-system

helm install --wait postgres-operator /tmp/postgres-operator/ --namespace sql-system

sleep 30
kubectl apply -f cloud/k8/data-services/postgres
sleep 40
kubectl wait pod -l=app=postgres --for=condition=Ready --timeout=360s
kubectl wait pod -l=statefulset.kubernetes.io/pod-name=postgres-0 --for=condition=Ready --timeout=360s
