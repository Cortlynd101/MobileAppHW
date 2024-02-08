using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NotesLib.Exceptions;

public class UnitializedDatabaseException : Exception
{
    public UnitializedDatabaseException()
    {
    }

    public UnitializedDatabaseException(string? message) : base(message)
    {
    }

    public UnitializedDatabaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UnitializedDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
