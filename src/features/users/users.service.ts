import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { UserEntity } from "../../core/entities/user.entity";
import { Repository } from "typeorm";
import { CreateUserDtoWithoutAvatar } from "../../core/contracts/dto/user/create-user.dto";
import { UserAlreadyExists } from "../../core/exceptions/user/user-already-exists";
import { FilesService } from "../files/files.service";
import { join } from "node:path";
import {
  MEDIA_FOLDER_NAME,
  SERVER_FOLDER,
} from "../../core/constants/default.constant";

@Injectable()
export class UsersService {
  constructor(
    @InjectRepository(UserEntity)
    private readonly repository: Repository<UserEntity>,
    private readonly filesService: FilesService,
  ) {}

  public async create(
    user: CreateUserDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<UserEntity> {
    const candidate: UserEntity | null = await this.repository.findOneBy({
      email: user.email,
    });
    if (candidate !== null) {
      throw new UserAlreadyExists("email", user.email);
    }

    const newUser: UserEntity = this.repository.create(user);
    if (avatar)
      newUser.avatarPath = await this.filesService.upload(
        avatar,
        join(SERVER_FOLDER, user.email, MEDIA_FOLDER_NAME),
        "avatar",
      );
    await this.repository.save(newUser);
    return newUser;
  }
}
