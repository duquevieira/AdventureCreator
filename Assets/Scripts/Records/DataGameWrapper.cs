using System;

[Serializable]
public class DataGameWrapper
{    
    public Game data;

    public DataGameWrapper(Game game) {
        this.data = game;
    }    
}
