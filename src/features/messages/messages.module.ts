import { Module } from "@nestjs/common";
import { MessagesService } from "@features/messages/messages.service";
import { MessagesController } from "@features/messages/messages.controller";
import { TypeOrmModule } from "@nestjs/typeorm";
import { MessageEntity } from "@entities/message.entity";

@Module({
  imports: [TypeOrmModule.forFeature([MessageEntity])],
  controllers: [MessagesController],
  providers: [MessagesService],
})
export class MessagesModule {}
