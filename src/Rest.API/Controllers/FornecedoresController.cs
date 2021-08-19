using API.Business.Intefaces;
using API.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rest.API.Data.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rest.API.Controllers
{
    [Route("api/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;
        public FornecedoresController(IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FornecedorDto>> ObterTodos()
        {
            var fornecedores =   _mapper.Map<IEnumerable<FornecedorDto>>(await _fornecedorRepository.ObterTodos());

            return fornecedores;
        }
    }
}
