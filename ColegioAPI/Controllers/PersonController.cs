using AutoMapper;
using Dominio;
using Dominio.Interfaces;
using Persistencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ColegioAPI.Dtos;
using ColegioAPI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ColegioAPI.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]
[ApiVersion("1.2")]

[Authorize]

public class PersonController : BaseApiController
{

    private readonly ColegioContext _context;
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public PersonController(IUnitOfWork unitOfWork, IMapper mapper, ColegioContext context)
    {
        this._unitofwork = unitOfWork;
        _mapper = mapper;
       _context = context;

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Person>>> Get()
    {
        var Con = await _unitofwork.Persons.GetAllAsync();
        return Ok(Con);
    }

    [HttpGet]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<PersonXcursoDto>>> Get11([FromQuery] Params personParams)
    {
        var pais = await _unitofwork.Persons.GetAllAsync(personParams.PageIndex, personParams.PageSize, personParams.Search);
        var lstPersons = _mapper.Map<List<PersonXcursoDto>>(pais.registros);
        return new Pager<PersonXcursoDto>(lstPersons, pais.totalRegistros, personParams.PageIndex, personParams.PageSize, personParams.Search);
    }
    /* 
        [HttpGet]
        [MapToApiVersion("1.2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<PersonxIncriptionDto>>> Get22([FromQuery] Params personParams)
        {
            var pais = await _unitofwork.Persons.GetAllAsync(personParams.PageIndex,personParams.PageSize,personParams.Search);
            var lstPersons = _mapper.Map<List<PersonxIncriptionDto>>(pais.registros);
            return new Pager<PersonxIncriptionDto>(lstPersons,pais.totalRegistros,personParams.PageIndex,personParams.PageSize,personParams.Search);
        } */


[HttpGet]
[MapToApiVersion("1.2")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<Pager<PersonxIncriptionDto>>> Get22([FromQuery] Params personParams)
{
    var pais = await _unitofwork.Persons.GetAllAsync(personParams.PageIndex,personParams.PageSize,personParams.Search);

    var lstPersons = new List<PersonxIncriptionDto>();
    foreach (var persona in pais.registros)
    {
        var personxIncriptionDto = new PersonxIncriptionDto();
        personxIncriptionDto.Id = persona.Id.ToString();
        personxIncriptionDto.Name = persona.Name;
        personxIncriptionDto.Lastname = persona.Lastname;
        personxIncriptionDto.BirthDate = persona.BirthDate.ToString();

       var inscripciones = await _context.Inscriptions.Where(i => i.IdPerson == persona.Id).ToListAsync();

//  aqui convierto las propiedades de mi entidad  a una lista Dto
var inscripcionesDtos = inscripciones.Select(i => new InscriptionDto
{
    Id = i.Id.ToString(),
    IdPerson = i.IdPerson.ToString(),
    Fecha_Inscription = i.Fecha_Inscription.ToString(),
    Id_Curso = i.Id_Curso.ToString()
}).ToList();

// Asignar la lista de mis dto a mi propiedad
personxIncriptionDto.Inscripciones = inscripcionesDtos;

        lstPersons.Add(personxIncriptionDto);
    }

    return new Pager<PersonxIncriptionDto>(lstPersons,pais.totalRegistros,personParams.PageIndex,personParams.PageSize,personParams.Search);
}





    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(int id)
    {
        var byidC = await _unitofwork.Persons.GetByIdAsync(id);
        return Ok(byidC);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Person>> Post(Person person)
    {
        this._unitofwork.Persons.Add(person);
        await _unitofwork.SaveAsync();
        if (person == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { id = person.Id }, person);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Person>> Put(int id, [FromBody] Person person)
    {
        if (person == null)
            return NotFound();
        _unitofwork.Persons.Update(person);
        await _unitofwork.SaveAsync();
        return person;
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var D = await _unitofwork.Persons.GetByIdAsync(id);
        if (D == null)
        {
            return NotFound();
        }
        _unitofwork.Persons.Remove(D);
        await _unitofwork.SaveAsync();
        return NoContent();
    }



}