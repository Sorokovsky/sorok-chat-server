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
import { MEDIA_FOLDER_NAME } from "@constants/default.constant";
import { UpdateChannelDtoWithoutFiles } from "@contracts/dto/channel/update-channel.dto";

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
    newChannel = await this.repository.merge(newChannel, withFiles).save();
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

  public async disconnectUser(
    userId: number,
    channelId: number,
  ): Promise<GetChannelDto> {
    const channel: GetChannelDto = await this.getById(channelId, true);
    channel.members = channel.members.filter(
      (member: UserEntity): boolean => member.id !== userId,
    );
    await this.repository.save(channel);
    return await this.getById(channelId, true);
  }

  public async update(
    id: number,
    channel: UpdateChannelDtoWithoutFiles,
    avatar?: Express.Multer.File,
    image?: Express.Multer.File,
  ): Promise<GetChannelDto> {
    const oldChannel: GetChannelDto = await this.getById(id, true);
    const withFiles: ChannelEntity = await this.processFiles(id, avatar, image);
    await this.repository
      .merge(oldChannel as ChannelEntity, channel, withFiles)
      .save();
    return await this.getById(id, true);
  }

  public async delete(id: number): Promise<GetChannelDto> {
    const channel: GetChannelDto = await this.getById(id, true);
    await this.repository.delete(id);
    return channel;
  }

  private async uploadAvatar(
    avatar: Express.Multer.File,
    channelFolder: string,
  ): Promise<string> {
    return await this.filesService.upload(
      avatar,
      join("channels", channelFolder, MEDIA_FOLDER_NAME),
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
      join("channels", channelFolder, MEDIA_FOLDER_NAME),
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
