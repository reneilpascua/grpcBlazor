using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using grpcservice.Data;
using GrpcService.Protos;
using Microsoft.Extensions.Logging;

namespace grpcservice.Services
{
    public class StudentsService : RemoteStudent.RemoteStudentBase
    {
        private readonly ILogger<StudentsService> _logger;
        private readonly SchoolDbContext _context;
        public StudentsService(ILogger<StudentsService> logger, SchoolDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override Task<StudentModel> GetStudentInfo(StudentLookupModel request, ServerCallContext context)
        {
            StudentModel output = new StudentModel();
            var student = _context.Students.Find(request.StudentId);
            _logger.LogInformation("Sending Student response");
            if (student != null)
            {
                output.StudentId = student.StudentId;
                output.FirstName = student.FirstName;
                output.LastName = student.LastName;
                output.School = student.School;
            }

            return Task.FromResult(output);
        }

        public override async Task<AllStudents> GetAllStudents(StudentLookupModel request, ServerCallContext context)
        {
            var students = _context.Students;
            var allstudents = new AllStudents();

            foreach (Models.Student s in students) {
                var sm = new StudentModel() {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    School = s.School
                };
                allstudents.Students.Add(sm);
            }

            _logger.LogInformation("Sending Student response");
            return await Task.FromResult(allstudents);
        }
    }
}