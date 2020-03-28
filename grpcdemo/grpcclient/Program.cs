using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using grpcservice;
using GrpcService.Protos;

namespace grpcclient
{
    class Program
    {
        static async Task Main(string[] args)
        {

            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // var input = new HelloRequest { Name = "Jane Bond" };

            // var channel = GrpcChannel.ForAddress("http://localhost:5000");
            // var client = new Greeter.GreeterClient(channel);

            // var reply = await client.SayHelloAsync(input);

            // Console.WriteLine(reply.Message);

            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var studentClient = new RemoteStudent.RemoteStudentClient(channel);
            var studentInput = new StudentLookupModel { StudentId = 33 };
            var studentReply = await studentClient.GetStudentInfoAsync(studentInput);
            Console.WriteLine($"{studentReply.FirstName} {studentReply.LastName}");

            var getAllReply = studentClient.GetAllStudents(studentInput);
            foreach (StudentModel s in getAllReply.Students) {
                Console.WriteLine($"{s.FirstName}");
            }
        }
    }
}
