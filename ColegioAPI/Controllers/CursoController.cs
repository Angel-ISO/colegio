using AutoMapper;
using Dominio;
using Dominio.Interfaces;
using Persistencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColegioAPIAPI.Controllers;

 public class CursoController : BaseApiController
{

     private readonly IUnitOfWork unitofwork;
     private readonly IMapper mapper;

    public CursoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitofwork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Curso>>> Get()
    {
        var Con = await  unitofwork.Courses.GetAllAsync();
        return Ok(Con);
    }

     [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
      public async Task<IActionResult> Get(int id)
    {
        var byidC = await  unitofwork.Courses.GetByIdAsync(id);
        return Ok(byidC);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Curso>> Post(Curso curso){
        this.unitofwork.Courses.Add(curso);
        await unitofwork.SaveAsync();
        if(curso == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post),new {id= curso.Id}, curso);
    }

     [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Curso>> Put(int id, [FromBody]Curso curso){
        if(curso == null)
            return NotFound();
        unitofwork.Courses.Update(curso);
        await unitofwork.SaveAsync();
        return curso;
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id){
        var D = await unitofwork.Courses.GetByIdAsync(id);
        if(D == null){
            return NotFound();
        }
        unitofwork.Courses.Remove(D);
        await unitofwork.SaveAsync();
        return NoContent();
    }


   
}