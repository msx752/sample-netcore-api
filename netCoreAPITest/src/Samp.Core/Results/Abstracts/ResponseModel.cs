﻿using Samp.Core.Interfaces;
using System.Collections.Generic;

namespace Samp.Core.Results.Abstracts
{
    public abstract class BaseResponseModel
    {
        public BaseResponseModel()
        {
            Stats = new();
        }

        public BaseResponseModel(IEnumerable<string> messages)
            : this()
        {
            Errors = new();
            Errors.AddRange(messages);
        }

        public BaseResponseModel(string message)
            : this()
        {
            Errors = new();
            Errors.Add(message);
        }

        public List<string> Errors { get; set; }
        public ResponseStatModel Stats { get; set; }
    }

    public class ResponseStatModel
    {
        public string RId { get; set; }
        public string ElapsedMilliseconds { get; set; }
        public string Time { get; set; }
    }

    public sealed class ResponseModel<T>
            : BaseResponseModel
        , IResponseModel<T>
        where T : class
    {
        public ResponseModel()
        {
        }

        public ResponseModel(IEnumerable<T> body)
        {
            Result = new();
            Result.AddRange(body);
        }

        public ResponseModel(T body)
        {
            Result = new();
            Result.Add(body);
        }

        public ResponseModel(IEnumerable<string> messages)
            : base(messages)
        {
        }

        public ResponseModel(string message)
            : base(message)
        {
        }

        public List<T> Result { get; set; }
    }

    public sealed class ResponseModel
        : BaseResponseModel
        , IResponseModel
    {
        public ResponseModel()
            : base()
        {
        }

        public ResponseModel(IEnumerable<string> messages)
            : base(messages)
        {
        }

        public ResponseModel(string message)
            : base(message)
        {
        }
    }
}