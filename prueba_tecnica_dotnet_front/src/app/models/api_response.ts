export class ValidateResponse {
  response: boolean;
  messages: string;
  statusCode: number;

  constructor(response: boolean, messages: string, statusCode: number){
    this.response = response;
    this.messages = messages;
    this.statusCode = statusCode;
  }
}
