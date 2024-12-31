import { Injectable } from "@nestjs/common";
import { GetChannelDto } from "@contracts/dto/channel/get-channel.dto";
import { InjectRepository } from "@nestjs/typeorm";
import { ChannelEntity } from "@entities/channel.entity";
import { Repository } from "typeorm";
import { CreateChannelDtoWithoutFiles } from "@contracts/dto/channel/create-channel.dto";
import { FilesService } from "@features/files/files.service";
import { join } from "node:path";
import { UserEntity } from "@entities/user.entity";
import { ChannelNotFoundException } from "@exceptions/channel/channel-not-found.exception";

@Injectable()
export class ChannelsService {
  constructor(
    @InjectRepository(ChannelEntity)
    private readonly repository: Repository<ChannelEntity>,
    private readonly filesService: FilesService,
  ) {}

  public async getById(
    id: number,
    withError: boolean = false,
  ): Promise<GetChannelDto> {
    const channel: GetChannelDto = await this.repository.findOne({
      where: { id },
      loadEagerRelations: true,
    });
    if (channel === null && withError) {
      throw new ChannelNotFoundException("id", id);
    }
    return channel;
  }

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
    channel: CreateChannelDtoWithoutFiles,
    avatar?: Express.Multer.File,
    image?: Express.Multer.File,
  ): Promise<GetChannelDto> {
    let newChannel: ChannelEntity = await this.repository
      .create(channel)
      .save();
    const withFiles: ChannelEntity = await this.processFiles(
      newChannel.id,
      avatar,
      image,
    );
    newChannel = this.repository.merge(newChannel, withFiles);
    newChannel = await this.repository.save(newChannel);
    return await this.connectUser(userId, newChannel.id);
  }

  public async connectUser(
    userId: number,
    channelId: number,
  ): Promise<GetChannelDto> {
    const channel: GetChannelDto = await this.getById(channelId, true);
    channel.members.push({ id: userId } as UserEntity);
    await this.repository.save(channel);
    return await this.getById(channelId, true);
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

  private async uploadBackground(
    background: Express.Multer.File,
    channelFolder: string,
  ): Promise<string> {
    return await this.filesService.upload(
      background,
      join("channels", channelFolder, `images`),
      "background",
      true,
    );
  }

  private async processFiles(
    id: number,
    avatar?: Express.Multer.File,
    image?: Express.Multer.File,
  ): Promise<ChannelEntity> {
    const result: ChannelEntity = {} as ChannelEntity;
    if (avatar) {
      result.avatarPath = await this.uploadAvatar(avatar, `${id}`);
    }
    if (image) {
      result.imagePath = await this.uploadBackground(image, `${id}`);
    }
    return result;
  }
}
