using System;

[Serializable]
public class DataWrapper
{    
    public Tale data;

    public DataWrapper(Tale Tale) {
        this.data = Tale;
    }    
}
