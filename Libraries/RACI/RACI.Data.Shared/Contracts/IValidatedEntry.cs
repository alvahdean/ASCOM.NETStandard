using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RACI.Data
{
    public interface IValidation<T>
    {
        /// <summary>
        /// Validates the object 'obj'
        /// </summary>
        /// <param name="obj">The object to validate</param>
        /// <returns>Returns result of CheckValid(obj) or throws an exception</returns>
        /// <exception cref="AggregateException">If CheckValid(obj) is false and ThrowOnInvalid is true</exception>
        /// <remarks>If an exception is thrown, details of validation errors (if provided) can be accessed via the InnerExceptions property</remarks>
        bool Validate(T obj);
        /// <summary>
        /// Validates the object 'obj'
        /// </summary>
        /// <param name="obj">The object to validate</param>
        /// <param name="errs">A collection to which validation details will be added</param>
        /// <returns>True if 'obj' is valid, otherwise false</returns>
        bool CheckValid(T obj, ICollection<ValidationException> errs);
        /// <summary>
        /// Specifies whether calls to Validate(...) will throw an exceptions if the object fails validation.
        /// </summary>
        bool ThrowOnInvalid { get; set; }
    }
}
