import csv
import requests
import json
import random
import string

def remover_caracteres_cpf(cpf_com_especiais):
    cpf_apenas_numeros = cpf_com_especiais.replace('.', '').replace('-', '')
    return cpf_apenas_numeros

def gerar_senha_segura(tamanho=12):
    caracteres = string.ascii_letters + string.digits + string.punctuation
    senha = random.choice(string.ascii_uppercase)
    senha += random.choice(string.ascii_lowercase)
    senha += random.choice(string.digits)
    senha += random.choice(string.punctuation)
    for _ in range(tamanho - 4):
        senha += random.choice(caracteres)
    senha_lista = list(senha)
    random.shuffle(senha_lista)
    return "".join(senha_lista)

def enviar_para_api(nome_do_arquivo_json, url_api):
    with open(nome_do_arquivo_json, 'r', encoding='utf-8') as arquivo:
        dados = json.load(arquivo)
        headers = {'Content-Type': 'application/json'}
        for item in dados:
            nome_completo = item['nome']
            email = item['email']
            telefone = item['celular']
            cpf = item['cpf']
            senha = gerar_senha_segura()
            login = cpf

            payload = json.dumps({
                "Name": nome_completo,
                "PhoneNumber": telefone,
                "EmailAddress": email,
                "Login": login,
                "Password": senha
            })

            try:
                response = requests.post(url_api, headers=headers, data=payload)
                response.raise_for_status() 
                print(f"Paciente '{nome_completo}' enviado com sucesso. Resposta: {response.text}, senha: {senha}")
            except requests.exceptions.RequestException as e:
                print(f"Erro ao enviar paciente '{nome_completo}': {e}")
            except Exception as e:
                print(f"Erro inesperado ao processar paciente '{nome_completo}': {e}")

# Exemplo de uso:
nome_do_arquivo_json = 'pacientes.json'
url_da_api = "http://localhost:8081/user/createPatientUser"
enviar_para_api(nome_do_arquivo_json, url_da_api)