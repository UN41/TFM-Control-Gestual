using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;

public class handDataLogger : MonoBehaviour
{
    public OVRSkeleton skeleton;
    public OVRHand hand;
    private float tiempoUltimaPoseGuardada = 0f;
    private float tiempoEntrePoses = 0.4f;
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;

    
    void Update()
    {
        // Comprueba si se ha asignado el componente OVRSkeleton
        if (skeleton == null)
        {
            Debug.LogError("El componente OVRSkeleton no está asignado en el inspector.");
            return;
        }
        if(hand.IsTracked && Time.time - tiempoUltimaPoseGuardada >= tiempoEntrePoses){
            // Obtiene la información de seguimiento de la mano izquierda
            OVRBone thumbBone = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip];
            OVRBone indexBone = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip];
            OVRBone middleBone = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_MiddleTip];
            OVRBone ringBone = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_RingTip];
            OVRBone pinkyBone = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_PinkyTip];

            Vector3 thumbTipPosition = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].Transform.position;
            Vector3 indexTipPosition = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
            Vector3 middleTipPosition = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_MiddleTip].Transform.position;
            Vector3 ringTipPosition = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_RingTip].Transform.position;
            Vector3 pinkyTipPosition = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_PinkyTip].Transform.position;

            // Obtiene la posición del centro de la palma
            Vector3 wristPosition = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_WristRoot].Transform.position;
            Vector3 middleKnucklePosition = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Middle1].Transform.position;
            Vector3 palmCenterPosition = (wristPosition + middleKnucklePosition) / 2f;

            // Calcula la distancia entre la punta de cada dedo y el centro de la palma
            float thumbDistance = Vector3.Distance(thumbTipPosition, palmCenterPosition);
            float indexDistance = Vector3.Distance(indexTipPosition, palmCenterPosition);
            float middleDistance = Vector3.Distance(middleTipPosition, palmCenterPosition);
            float ringDistance = Vector3.Distance(ringTipPosition, palmCenterPosition);
            float pinkyDistance = Vector3.Distance(pinkyTipPosition, palmCenterPosition);

            float distance1 = Vector3.Distance(thumbTipPosition, indexTipPosition);
            float distance2 = Vector3.Distance(thumbTipPosition, pinkyTipPosition);

            // Registra los datos en la consola

            WWWForm form = new WWWForm();
            form.AddField("thumbDistance", thumbDistance.ToString());
            form.AddField("indexDistance", indexDistance.ToString());
            form.AddField("middleDistance", middleDistance.ToString());
            form.AddField("ringDistance", ringDistance.ToString());
            form.AddField("pinkyDistance", pinkyDistance.ToString());
            form.AddField("distance1", distance1.ToString());
            form.AddField("distance2", distance2.ToString());
            form.AddField("T0", thumbBone.Transform.rotation[0].ToString());
            form.AddField("T1", thumbBone.Transform.rotation[1].ToString());
            form.AddField("T2", thumbBone.Transform.rotation[2].ToString());
            form.AddField("T3", thumbBone.Transform.rotation[3].ToString());
            form.AddField("I0", indexBone.Transform.rotation[0].ToString());
            form.AddField("I1", indexBone.Transform.rotation[1].ToString());
            form.AddField("I2", indexBone.Transform.rotation[2].ToString());
            form.AddField("I3", indexBone.Transform.rotation[3].ToString());
            form.AddField("M0", middleBone.Transform.rotation[0].ToString());
            form.AddField("M1", middleBone.Transform.rotation[1].ToString());
            form.AddField("M2", middleBone.Transform.rotation[2].ToString());
            form.AddField("M3", middleBone.Transform.rotation[3].ToString());
            form.AddField("R0", ringBone.Transform.rotation[0].ToString());
            form.AddField("R1", ringBone.Transform.rotation[1].ToString());
            form.AddField("R2", ringBone.Transform.rotation[2].ToString());
            form.AddField("R3", ringBone.Transform.rotation[3].ToString());
            form.AddField("P0", pinkyBone.Transform.rotation[0].ToString());
            form.AddField("P1", pinkyBone.Transform.rotation[1].ToString());
            form.AddField("P2", pinkyBone.Transform.rotation[2].ToString());
            form.AddField("P3", pinkyBone.Transform.rotation[3].ToString());

            // Enviar la solicitud POST al servidor
            StartCoroutine(PostRequest(form));
            tiempoUltimaPoseGuardada = Time.time;
        }
    }

        IEnumerator PostRequest(WWWForm form)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://192.168.1.169/predict3",form)){
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                //Debug.Log("Solicitud de POST enviada correctamente.");
                string responseText = request.downloadHandler.text;
                int startIndex = responseText.IndexOf('"') + 1;
                int endIndex = responseText.LastIndexOf('"');
                string output = responseText.Substring(startIndex, endIndex - startIndex);
                Debug.Log("Predicción: " + output);
                CambiarValorAnimator(output);
            }
            else
            {
                Debug.LogError("Error al enviar la solicitud POST: " + request.error);
            }
        }

    }
    public void CambiarValorAnimator(string valor)
    {
        int singValue;
        // Mapear los valores de string a int utilizando una estructura condicional
        if (valor == "stop")
        {
            singValue = 0;
        }
        else if (valor == "down")
        {
            singValue = 1;
        }
        else if (valor == "up")
        {
            singValue = 2;
        }
        else if (valor == "go")
        {
            singValue = 3;
        }
        else
        {
            // Valor no reconocido, realizar alguna acción adicional si es necesario
            return;
        }

        animator1.SetInteger("sing", singValue);
        animator2.SetInteger("sing", singValue);
        animator3.SetInteger("sing", singValue);
    }
}