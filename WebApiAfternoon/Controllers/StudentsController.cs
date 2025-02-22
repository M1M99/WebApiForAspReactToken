﻿using Microsoft.AspNetCore.Mvc;
using WebApiAfternoon.Dtos;
using WebApiAfternoon.Entities;
using WebApiAfternoon.Repositories.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiAfternoon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: api/<StudentsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> Get()
        {
            var students = await _studentRepository.GetAll();
            var dataToReturn = students.Select(s => new StudentDto
            {
                 Age = s.Age,
                 Id = s.Id,
                 Fullname=s.Fullname,
                 Score = s.Score,
                 SeriaNo=s.SeriaNo
            });
            return Ok(dataToReturn);
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> Get(int id)
        {
            var item = await _studentRepository.Get(s => s.Id == id);
            if (item == null) return NotFound();
            var dataToReturn = new StudentDto
            {
                Id = item.Id,
                Age=item.Age,
                Fullname=item.Fullname,
                Score = item.Score,
                SeriaNo=item.SeriaNo
            };
            return Ok(dataToReturn);
        }

        // POST api/<StudentsController>
        [HttpPost]
        public async Task<ActionResult<StudentDto>> Post([FromBody] StudentAddDto item)
        {
            var student = new Student
            {
                Age = item.Age,
                Fullname = item.Fullname,
                Score = item.Score,
                SeriaNo = item.SeriaNo
            };
            var returnedEntity=await _studentRepository.Add(student);

            var dataToReturn = new StudentDto
            {
                Id = returnedEntity.Id,
                Age = returnedEntity.Age,
                Fullname = returnedEntity.Fullname,
                Score = returnedEntity.Score,
                SeriaNo = returnedEntity.SeriaNo
            };
            return Ok(dataToReturn);
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
