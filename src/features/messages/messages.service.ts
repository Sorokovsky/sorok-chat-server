import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { MessageEntity } from "@entities/message.entity";
import { Repository } from "typeorm";
import { CreateMessageDto } from "@contracts/dto/message/create-message.dto";
import { GetMessageDto } from "@contracts/dto/message/get-message.dto";

@Injectable()
export class MessagesService {
  constructor(
    @InjectRepository(MessageEntity)
    private readonly repository: Repository<MessageEntity>,
  ) {}

  public async getByChat(channelId: number): Promise<MessageEntity[]> {
    return await this.repository.find({
      where: {
        channel: { id: channelId },
      },
    });
  }

  public async getById(id: number): Promise<GetMessageDto> {
    return (await this.repository.findOne({
      where: { id },
      loadEagerRelations: true,
    })) as GetMessageDto;
  }

  public async create(
    userId: number,
    channelId: number,
    text: CreateMessageDto,
  ): Promise<GetMessageDto> {
    const { id } = await this.repository
      .create({
        ...text,
        author: { id: userId },
        channel: { id: channelId },
      })
      .save();
    return await this.getById(id);
  }
}
