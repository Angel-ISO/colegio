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
[Authorize]


 public class InscriptionController : BaseApiController
{

     private readonly IUnitOfWork _unitofwork;
     private readonly IMapper _mapper;

    public InscriptionController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitofwork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<InscriptionDto>>> Get()
    {
        var Con = await  _unitofwork.Inscriptions.GetAllAsync();
        return _mapper.Map<List<InscriptionDto>>(Con);
    }


  




    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Inscription>> Post(InscriptionDto inscriptionDto){
        var inscription = _mapper.Map<Inscription>(inscriptionDto);
        this._unitofwork.Inscriptions.Add(inscription);
        await _unitofwork.SaveAsync();
        if(inscription == null)
        {
            return BadRequest();
        }
        inscriptionDto.Id = inscription.Id.ToString();
        return CreatedAtAction(nameof(Post),new {id= inscriptionDto.Id}, inscriptionDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Inscription>> Put(int id, [FromBody]Inscription inscription){
        if(inscription == null)
            return NotFound();
        _unitofwork.Inscriptions.Update(inscription);
        await _unitofwork.SaveAsync();
        return inscription;
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id){
        var D = await _unitofwork.Inscriptions.GetByIdAsync(id);
        if(D == null){
            return NotFound();
        }
        _unitofwork.Inscriptions.Remove(D);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
