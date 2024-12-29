import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { UserEntity } from "../../core/entities/user.entity";
import { FindOptionsWhere, Repository } from "typeorm";
import { CreateUserDtoWithoutAvatar } from "../../core/contracts/dto/user/create-user.dto";
import { UserAlreadyExists } from "../../core/exceptions/user/user-already-exists";
import { FilesService } from "../files/files.service";
import { join } from "node:path";
import { MEDIA_FOLDER_NAME } from "../../core/constants/default.constant";
import { UserNotFoundException } from "../../core/exceptions/user/user-not-found.exception";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";
import { UpdateUserDtoWithoutAvatar } from "../../core/contracts/dto/user/update-user.dto";
import { ColumnMetadata } from "typeorm/metadata/ColumnMetadata";

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
    return await this.getBy({ email }, notFoundError);
  }

  public async getById(
    id: number,
    notFoundError: boolean = false,
  ): Promise<GetUserDto | null> {
    return await this.getBy({ id }, notFoundError);
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
    delete newUser["password"];
    return newUser;
  }

  public async update(
    id: number,
    newest: UpdateUserDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    let candidate: UserEntity | null = await this.repository.findOne({
      where: {
        id,
      },
      select: this.getColumnsName(),
    });
    console.log(candidate);
    candidate = this.repository.merge(candidate, newest);
    console.log(candidate);
    if (avatar) {
      candidate.avatarPath = await this.uploadAvatar(avatar, `${candidate.id}`);
    }
    try {
      await this.repository.save(candidate);
    } catch {
      throw new UserAlreadyExists("email", candidate.email);
    }
    delete candidate["password"];
    return candidate;
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

  private async getBy(
    findOptions: FindOptionsWhere<UserEntity>,
    notFoundError: boolean = false,
  ): Promise<GetUserDto | null> {
    const user: GetUserDto = await this.repository.findOneBy(findOptions);
    if (user === null && notFoundError) {
      throw new UserNotFoundException("email", findOptions.email);
    }
    return user;
  }

  private getColumnsName(): (keyof UserEntity)[] {
    return this.repository.metadata.columns.map(
      (col: ColumnMetadata): string => col.propertyName,
    ) as (keyof UserEntity)[];
  }
}
