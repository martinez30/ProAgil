using System.Net.Http.Headers;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.API.DTOs;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repository;

        public IMapper _mapper { get; }

        public EventoController(IProAgilRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }        

        [HttpGet]
        public async Task<IActionResult> Get(){
            try
            {
                var eventos = await  _repository.GetAllEventoAsync(true);

                var results = _mapper.Map<IEnumerable<EventoDto>>(eventos);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(){
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("","");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0){
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\"", " ")).ToString();

                    using(var stream = new FileStream(fullPath, FileMode.Create)){
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest("Erro ao tentar realizar upload");
        }

          [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId){
            try
            {
                var evento = await  _repository.GetEventoAsyncById(EventoId, true);

                var result = _mapper.Map<EventoDto>(evento);

                return Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

           [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema){
            try
            {
                var eventos = await  _repository.GetAllEventoAsyncByTema(tema, true);

                 var results = _mapper.Map<IEnumerable<EventoDto>>(eventos);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

          [HttpPost]
        public async Task<IActionResult> Post(EventoDto model){
            try
            {

                var evento = _mapper.Map<Evento>(model);

                _repository.Add(evento);

                if(await _repository.SaveChangesAsync())
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

            [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto model){
            try
            {
                var evento = await _repository.GetEventoAsyncById(EventoId, false);
                if(evento is null) return NotFound();

                _mapper.Map(model, evento);

                Console.WriteLine(model);
                Console.WriteLine(evento);
                
                _repository.Update(evento);
                
                if(await _repository.SaveChangesAsync()){
                    return Ok();
                }

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }

          [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId){
            try
            {
                var evento = await _repository.GetEventoAsyncById(EventoId, false);
                if(evento is null) return NotFound();

                _repository.Delete(evento);
                
                if(await _repository.SaveChangesAsync()){
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }
    }
}