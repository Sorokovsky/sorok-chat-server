import { Injectable } from "@nestjs/common";
import { GetChannelDto } from "@contracts/dto/channel/get-channel.dto";
import { InjectRepository } from "@nestjs/typeorm";
import { ChannelEntity } from "@entities/channel.entity";
import { Repository } from "typeorm";
import { CreateChannelDtoWithoutAvatar } from "@contracts/dto/channel/create-channel.dto";
import { FilesService } from "@features/files/files.service";
import { join } from "node:path";
import { UsersService } from "@features/users/users.service";
import { GetUserDto } from "@contracts/dto/user/get-user.dto";
import { UserEntity } from "@entities/user.entity";

@Injectable()
export class ChannelsService {
  constructor(
    @InjectRepository(ChannelEntity)
    private readonly repository: Repository<ChannelEntity>,
    private readonly filesService: FilesService,
    private readonly usersService: UsersService,
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
      members: [],
    });
    if (avatar) {
      newChannel.avatarPath = await this.uploadAvatar(
        avatar,
        `${newChannel.id}`,
      );
    }
    newChannel = await this.repository.save(newChannel);
    const user: UserEntity = (await this.usersService.getById(
      userId,
      false,
      true,
    )) as UserEntity;
    newChannel.members = [...newChannel.members, user];
    await this.repository.save(newChannel);
    return newChannel;
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
}
