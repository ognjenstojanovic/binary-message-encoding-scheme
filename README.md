# binary-message-encoding-scheme

Assumptions:

1. Values used for Header and Paylaod separation can't be used in the Header name or Value fields:
  - Message header name can't contain ':'
  - Message header name and value can't contain '\r\n' and '\n\r\'
2. Max number of headers is 63, but one header is reserved for the value that separates the Header and the Payload
