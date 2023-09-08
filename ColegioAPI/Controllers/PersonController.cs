using AutoMapper;
using Dominio;
using Dominio.Interfaces;
using Persistencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

 public class PersonController : BaseApiController
{

     private readonly IUnitOfWork unitofwork;
     private readonly IMapper mapper;

    public PersonController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitofwork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Person>>> Get()
    {
        var Con = await  unitofwork.Persons.GetAllAsync();
        return Ok(Con);
    }

     [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
      public async Task<IActionResult> Get(int id)
    {
        var byidC = await  unitofwork.Persons.GetByIdAsync(id);
        return Ok(byidC);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Person>> Post(Person person){
        this.unitofwork.Persons.Add(person);
        await unitofwork.SaveAsync();
        if(person == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post),new {id= person.Id}, person);
    }

     [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Person>> Put(int id, [FromBody]Person person){
        if(person == null)
            return NotFound();
        unitofwork.Persons.Update(person);
        await unitofwork.SaveAsync();
        return person;
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id){
        var D = await unitofwork.Persons.GetByIdAsync(id);
        if(D == null){
            return NotFound();
        }
        unitofwork.Persons.Remove(D);
        await unitofwork.SaveAsync();
        return NoContent();
    }


   
}