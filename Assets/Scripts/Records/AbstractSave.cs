using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AbstractSave : MonoBehaviour
{
    private string _endpointPostPath = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/document-schema/{0}/documents";
    private string _endpointDeletePath = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/";
    private string _endpointGetPath = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/documents/";
    public StoryEngineScript Story;
    public PlayerHandlerScript PlayerHandler;

    public async Task<string> PostNewSaveAsync(string json, string id)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsync(string.Format(_endpointPostPath, id), new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            return await TreatGetResponseAsync(response);
        }
    }

    public async Task<string> PutSaveAsync(string json, string id, string SaveId)
    {
        using (var httpClient = new HttpClient())
        {
            await DeleteSaveAsync(SaveId);
            return await PostNewSaveAsync(json, id);
        }
    }

    public async Task DeleteSaveAsync(string SaveId)
    {
        using (var httpClient = new HttpClient())
        {
            await httpClient.DeleteAsync(_endpointDeletePath + SaveId);
        }
    }

    public async Task<string> GetSaveAsync(string SaveId)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(_endpointGetPath + SaveId);
            return await TreatGetResponseAsync(response);
        }
    }



    private async Task<string> TreatGetResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            Debug.Log("Error: " + response.StatusCode);
            return null;
        }
    }    
    
    public string GetSave(string SaveId) {
        var requestGet = WebRequest.Create(_endpointGetPath+SaveId);
        requestGet.Method = "GET";
        return TreatGetResponse(requestGet);
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
}
