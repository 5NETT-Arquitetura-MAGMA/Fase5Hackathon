import csv
import requests
import json
import random
import string

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

def enviar_para_api(nome_arquivo_csv, url_api):
    with open(nome_arquivo_csv, 'r', newline='') as arquivo_csv:
        leitor_csv = csv.DictReader(arquivo_csv, delimiter=';')
        headers = {'Content-Type': 'application/json'}
        for linha in leitor_csv:
            nome_completo = linha['nome completo']
            email = linha['email']
            telefone = linha['telefone de contato']
            crm = linha['CRM']
            especialidade = linha['Especialidade']
            senha = gerar_senha_segura()
            login = crm

            payload = json.dumps({
                "Name": nome_completo,
                "PhoneNumber": telefone,
                "EmailAddress": email,
                "Login": login,
                "Password": senha,
                "Specialty": especialidade
            })

            try:
                response = requests.post(url_api, headers=headers, data=payload)
                response.raise_for_status() 
                print(f"Médico '{nome_completo}' enviado com sucesso. Resposta: {response.text}")
            except requests.exceptions.RequestException as e:
                print(f"Erro ao enviar médico '{nome_completo}': {e}")
            except Exception as e:
                print(f"Erro inesperado ao processar médico '{nome_completo}': {e}")

# Exemplo de uso:
nome_do_arquivo_csv = 'medicos.csv'
url_da_api = "http://localhost:5031/user/createMedicUser"
enviar_para_api(nome_do_arquivo_csv, url_da_api)