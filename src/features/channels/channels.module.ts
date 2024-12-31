import { Module } from "@nestjs/common";
import { ChannelsService } from "@features/channels/channels.service";
import { ChannelsController } from "@features/channels/channels.controller";
import { TypeOrmModule } from "@nestjs/typeorm";
import { ChannelEntity } from "@entities/channel.entity";
import { FilesService } from "@features/files/files.service";

@Module({
  imports: [TypeOrmModule.forFeature([ChannelEntity])],
  controllers: [ChannelsController],
  providers: [ChannelsService, FilesService],
})
export class ChannelsModule {}
