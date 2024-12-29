import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { UserEntity } from "../../core/entities/user.entity";
import { Repository } from "typeorm";
import { CreateUserDtoWithoutAvatar } from "../../core/contracts/dto/user/create-user.dto";
import { UserAlreadyExists } from "../../core/exceptions/user/user-already-exists";
import { FilesService } from "../files/files.service";
import { join } from "node:path";
import { MEDIA_FOLDER_NAME } from "../../core/constants/default.constant";
import { UserNotFoundException } from "../../core/exceptions/user/user-not-found.exception";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";

@Injectable()
export class UsersService {
  constructor(
    @InjectRepository(UserEntity)
    private readonly repository: Repository<UserEntity>,
    private readonly filesService: FilesService,
  ) {}

  public async getByEmail(
    email: string,
    notFoundError: boolean = false,
  ): Promise<GetUserDto | null> {
    const user: GetUserDto = await this.repository.findOneBy({ email });
    if (user === null && notFoundError) {
      throw new UserNotFoundException("email", email);
    }
    return user;
  }

  public async create(
    user: CreateUserDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    const candidate: GetUserDto | null = await this.getByEmail(user.email);
    if (candidate !== null) {
      throw new UserAlreadyExists("email", user.email);
    }

    const newUser: GetUserDto = this.repository.create(user);
    await this.repository.save(newUser);
    if (avatar) {
      newUser.avatarPath = await this.uploadAvatar(avatar, `${newUser.id}`);
    }
    await this.repository.save(newUser);
    return newUser;
  }

  private async uploadAvatar(
    avatar: Express.Multer.File,
    userFolder: string,
  ): Promise<string> {
    return await this.filesService.upload(
      avatar,
      join("users", userFolder, MEDIA_FOLDER_NAME),
      "avatar",
    );
  }
}
