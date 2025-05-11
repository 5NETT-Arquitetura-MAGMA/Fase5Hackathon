import random
import string

def gerar_lista_crm(numero_resultados):
    """
    Gera uma lista de códigos CRM formatados.

    Args:
        numero_resultados (int): O número de códigos CRM a serem gerados.

    Returns:
        list: Uma lista contendo os códigos CRM gerados.
    """
    estados_brasil = [
        "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
        "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
        "RS", "RO", "RR", "SC", "SP", "SE", "TO"
    ]
    lista_crm = []
    for _ in range(numero_resultados):
        estado = random.choice(estados_brasil)
        sequencia = ''.join(random.choices(string.digits, k=6))
        codigo_crm = f"CRM/{estado} {sequencia}"
        lista_crm.append(codigo_crm)
    return lista_crm

# Exemplo de uso: gerar 5 códigos CRM
numero_desejado = 89
lista_de_codigos = gerar_lista_crm(numero_desejado)
for codigo in lista_de_codigos:
    print(codigo)