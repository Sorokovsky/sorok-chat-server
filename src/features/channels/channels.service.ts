import { Injectable } from "@nestjs/common";
import { GetChannelDto } from "@contracts/dto/channel/get-channel.dto";
import { InjectRepository } from "@nestjs/typeorm";
import { ChannelEntity } from "@entities/channel.entity";
import { Repository } from "typeorm";
import { CreateChannelDtoWithoutAvatar } from "@contracts/dto/channel/create-channel.dto";
import { FilesService } from "@features/files/files.service";
import { join } from "node:path";
import { UserEntity } from "@entities/user.entity";

@Injectable()
export class ChannelsService {
  constructor(
    @InjectRepository(ChannelEntity)
    private readonly repository: Repository<ChannelEntity>,
    private readonly filesService: FilesService,
  ) {}

  public async getChannels(userId: number): Promise<GetChannelDto[]> {
    return (await this.repository.find({
      where: {
        members: {
          id: userId,
        },
      },
    })) as GetChannelDto[];
  }

  public async createChannel(
    userId: number,
    channel: CreateChannelDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<GetChannelDto> {
    let newChannel: ChannelEntity = this.repository.create({
      ...channel,
    });
    if (avatar) {
      newChannel.avatarPath = await this.uploadAvatar(
        avatar,
        `${newChannel.id}`,
      );
    }
    newChannel = await this.repository.save(newChannel);
    return await this.connectUser(userId, newChannel.id);
  }

  private async uploadAvatar(
    avatar: Express.Multer.File,
    channelFolder: string,
  ): Promise<string> {
    return await this.filesService.upload(
      avatar,
      join("channels", channelFolder, `images`),
      "avatar",
      true,
    );
  }

  public async connectUser(
    userId: number,
    channelId: number,
  ): Promise<GetChannelDto> {
    const channel: ChannelEntity = await this.repository.findOne({
      where: {
        id: channelId,
      },
    });
    channel.members.push({ id: userId } as UserEntity);
    await this.repository.save(channel);
    return await this.repository.findOne({
      where: { id: channelId },
      loadEagerRelations: true,
    });
  }
}
