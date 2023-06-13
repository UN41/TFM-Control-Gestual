from flask import render_template
from app import app
from flask import Flask, request, jsonify
import pickle
import numpy
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import GridSearchCV

pickle_model=pickle.load(open("/app/mydata/rf.pickle","rb"))

def parseData(data):
    thumbDistance = float(data['thumbDistance'].replace(',', '.'))
    indexDistance = float(data['indexDistance'].replace(',', '.'))
    middleDistance = float(data['middleDistance'].replace(',', '.'))
    ringDistance = float(data['ringDistance'].replace(',', '.'))
    pinkyDistance = float(data['pinkyDistance'].replace(',', '.'))
    distance1 = float(data['distance1'].replace(',', '.'))
    distance2 = float(data['distance2'].replace(',', '.'))

    T0 = float(data['T0'].replace(',', '.'))
    T1 = float(data['T1'].replace(',', '.'))
    T2 = float(data['T2'].replace(',', '.'))
    T3 = float(data['T3'].replace(',', '.'))

    I0 = float(data['I0'].replace(',', '.'))
    I1 = float(data['I1'].replace(',', '.'))
    I2 = float(data['I2'].replace(',', '.'))
    I3 = float(data['I3'].replace(',', '.'))

    M0 = float(data['M0'].replace(',', '.'))
    M1 = float(data['M1'].replace(',', '.'))
    M2 = float(data['M2'].replace(',', '.'))
    M3 = float(data['M3'].replace(',', '.'))

    R0 = float(data['R0'].replace(',', '.'))
    R1 = float(data['R1'].replace(',', '.'))
    R2 = float(data['R2'].replace(',', '.'))
    R3 = float(data['R3'].replace(',', '.'))

    P0 = float(data['P0'].replace(',', '.'))
    P1 = float(data['P1'].replace(',', '.'))
    P2 = float(data['P2'].replace(',', '.'))
    P3 = float(data['P3'].replace(',', '.'))

    values = [
        T0,
        T1,
        T2,
        T3,
        I0,
        I1,
        I2,
        I3,
        M0,
        M1,
        M2,
        M3,
        R0,
        R1,
        R2,
        R3,
        P0,
        P1,
        P2,
        P3,
        thumbDistance,
        indexDistance,
        middleDistance,
        ringDistance,
        pinkyDistance,
        distance1,
        distance2
    ]

    return numpy.array(values)


@app.route('/')
def home():
    return render_template("index.html")
    
    
@app.route('/test', methods=["GET"])
def test():
   return {"test":"hola amigo"}

@app.route('/predict', methods=["POST"])
def predict():
   data = request.json
   return jsonify(data)

@app.route('/predict2', methods=["POST"])
def predict2():
   data=request.json
   print(data)
   return {"test":"hola amigo"}

@app.route('/predict3', methods=['POST'])
def predict3():
    data = request.form  # Obtener los datos enviados en el formulario
    
    values = parseData(data)
    print("----:)-----")
    result = pickle_model.predict(values.reshape(1,-1))
    print("----:)-----")
    print("->" + result)
    result_serializable = result.tolist()
    
    # Enviar la respuesta a Unity
    return jsonify(result_serializable)