using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Application.Enum
{
    public enum SaveActionStatus
    {
        Success,
        PrimaryKeyViolation,
        ForeignKeyViolation,
        UnknownError,
        validationError,
        PartiallySaved
    }
}
