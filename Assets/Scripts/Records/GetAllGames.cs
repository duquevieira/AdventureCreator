using System.Collections.Generic;
using System;

[Serializable]
public class GetAllGames
{
    public string _rid;
    public List<DocumentGame> Documents;
    public int _count;

    public GetAllGames() {
        Documents = new List<DocumentGame>();
    }
}
