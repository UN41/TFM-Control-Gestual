FROM python:3.9-slim-buster
EXPOSE 80
COPY requirements.txt /
RUN pip3 install -r /requirements.txt

COPY . /app
WORKDIR /app

ENTRYPOINT ["./gunicorn.sh"]
