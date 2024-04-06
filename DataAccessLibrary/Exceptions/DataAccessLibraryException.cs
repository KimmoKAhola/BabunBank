namespace DataAccessLibrary.Exceptions;

public class DataAccessLibraryException : Exception
{
    public DataAccessLibraryException(string error)
        : base(error) { }
}
