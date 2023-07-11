using System;

[Serializable]
public class DataTaleWrapper
{    
    public Tale data;

    public DataTaleWrapper(Tale Tale) {
        this.data = Tale;
    }    
}
