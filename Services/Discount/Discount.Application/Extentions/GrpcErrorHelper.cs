using Grpc.Core;
using Google.Rpc;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf;
using Googlestatus = Google.Rpc.Status;
using Grpcstatus = Grpc.Core.Status;

namespace Discount.Application.Extentions
{
    public static class GrpcErrorHelper
    {
        public static RpcException CreateValidationException(Dictionary<string, string> fieldErrors)
        {
            var fieldViolations = new List<BadRequest.Types.FieldViolation>();
            foreach (var error in fieldErrors)
            {
                fieldViolations.Add(new BadRequest.Types.FieldViolation
                {
                    Field = error.Key,
                    Description = error.Value
                });
            }
            var badRequest = new BadRequest
            {
                FieldViolations = { fieldViolations }
            };
            var status = new Googlestatus
            {
                Code = (int)StatusCode.InvalidArgument,
                Message = "Invalid request parameters",
                Details = { Any.Pack(badRequest) }
            };
            var trailers = new Metadata
            {
                { "grpc-status-details-bin", status.ToByteArray() }
            };
            return new RpcException(new Grpcstatus(StatusCode.InvalidArgument, status.Message), trailers);
        }
    }
}
