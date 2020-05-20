namespace Framework.Exceptions
{
    public enum ExceptionType
    {        
        Business,
        Runtime,
        Database,
        Technical = Runtime | Database 
    }
}