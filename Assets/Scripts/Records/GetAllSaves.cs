using System.Collections.Generic;
using System;

[Serializable]
public class GetAllSaves
{
    public string _rid;
    public List<Document> Documents;
    public int _count;

    public GetAllSaves() {
        Documents = new List<Document>();
    }
}
