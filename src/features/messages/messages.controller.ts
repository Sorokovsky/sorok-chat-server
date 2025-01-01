import {
  Body,
  Controller,
  Get,
  Param,
  ParseIntPipe,
  Post,
} from "@nestjs/common";
import { MessagesService } from "@features/messages/messages.service";
import { ApiCreatedResponse, ApiOkResponse } from "@nestjs/swagger";
import { GetMessageDto } from "@contracts/dto/message/get-message.dto";
import { Auth } from "@decorators/auth.decorator";
import { CurrentUser } from "@decorators/current-user.decorator";
import { CreateMessageDto } from "@contracts/dto/message/create-message.dto";

@Controller("messages")
export class MessagesController {
  constructor(private readonly messagesService: MessagesService) {}

  @Auth()
  @Post(":chatId")
  @ApiCreatedResponse({
    type: GetMessageDto,
    description: "Created",
  })
  async createMessage(
    @CurrentUser("id", new ParseIntPipe()) userId: number,
    @Param("chatId", new ParseIntPipe()) chatId: number,
    @Body() message: CreateMessageDto,
  ): Promise<GetMessageDto> {
    return await this.messagesService.create(userId, chatId, message);
  }

  @Auth()
  @Get(":chatId")
  @ApiOkResponse({
    type: GetMessageDto,
    description: "Found",
  })
  public async getByChat(
    @Param("chatId", new ParseIntPipe()) chatId: number,
  ): Promise<GetMessageDto[]> {
    return await this.messagesService.getByChat(chatId);
  }
}
