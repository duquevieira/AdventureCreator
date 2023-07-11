using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;

public abstract class AbstractSave : MonoBehaviour
{
    private string _endpointPostPath = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/document-schema/%s/documents";
    private string _endpointDeletePath = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/";
    private string _endpointGetPath = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/documents/";
    public StoryEngineScript Story;

    public string PostNewSave(string json, string id)
    {
        var request = WebRequest.Create(string.Format(_endpointPostPath, id));
        request.Method = "POST";
        request.ContentType = "application/json";
        return TreatResponse(json, request);
    }

    public string PutSave(string json, string id, string SaveId)
    {
        var requestDelete = WebRequest.Create(_endpointDeletePath+SaveId);
        requestDelete.Method = "DELETE";
        requestDelete.ContentType = "application/json";
        TreatResponse(json, requestDelete);
        var requestPost = WebRequest.Create(string.Format(_endpointPostPath, id));
        requestPost.Method = "POST";
        requestPost.ContentType = "application/json";
        return TreatResponse(json, requestPost);
    }

    public static string TreatResponse(string json, WebRequest request)
    {
        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(json);
        }
        return TreatGetResponse(request);
    }

    public static string TreatGetResponse(WebRequest request) 
    {
        try
        {
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            return reader.ReadToEnd();
        }
        catch (WebException e)
        {
            Debug.Log("Error: " + e.Message);
        }
        return null;
    }

    public string GetSave(string SaveId) {
        var requestGet = WebRequest.Create(_endpointGetPath+SaveId);
        requestGet.Method = "GET";
        return TreatGetResponse(requestGet);
    }
}
