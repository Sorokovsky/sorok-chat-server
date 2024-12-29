import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { UserEntity } from "../../core/entities/user.entity";
import { FindOptionsWhere, Repository } from "typeorm";
import { CreateUserDtoWithoutAvatar } from "../../core/contracts/dto/user/create-user.dto";
import { UserAlreadyExists } from "../../core/exceptions/user/user-already-exists";
import { FilesService } from "../files/files.service";
import { join } from "node:path";
import { MEDIA_FOLDER_NAME } from "../../core/constants/default.constant";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";
import { UpdateUserDtoWithoutAvatar } from "../../core/contracts/dto/user/update-user.dto";
import { ColumnMetadata } from "typeorm/metadata/ColumnMetadata";
import { UserNotFoundException } from "../../core/exceptions/user/user-not-found.exception";
import { PasswordService } from "../password/password.service";

@Injectable()
export class UsersService {
  constructor(
    @InjectRepository(UserEntity)
    private readonly repository: Repository<UserEntity>,
    private readonly filesService: FilesService,
    private readonly passwordService: PasswordService,
  ) {}

  public async getByEmail(
    email: string,
    notFoundError: boolean = false,
  ): Promise<GetUserDto | null> {
    const user: GetUserDto = await this.getBy({ email });
    if (user === null && notFoundError) {
      throw new UserNotFoundException("email", email);
    }
    return user;
  }

  public async getById(
    id: number,
    notFoundError: boolean = false,
  ): Promise<GetUserDto | null> {
    const user: GetUserDto = await this.getBy({ id });
    if (user === null && notFoundError) {
      throw new UserNotFoundException("id", id);
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

    const newUser: GetUserDto = this.repository.create({
      ...user,
      password: await this.passwordService.hash(user.password),
    });
    await this.repository.save(newUser);
    if (avatar) {
      newUser.avatarPath = await this.uploadAvatar(avatar, `${newUser.id}`);
    }
    await this.repository.save(newUser);
    delete newUser["password"];
    return newUser;
  }

  public async delete(id: number): Promise<GetUserDto> {
    const user: GetUserDto = await this.getById(id);
    if (user === null) {
      throw new UserAlreadyExists("id", id);
    }
    await this.repository.delete(id);
    return user;
  }

  public async update(
    id: number,
    newest: UpdateUserDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    const columns: (keyof UserEntity)[] = this.getColumnsName();
    let candidate: UserEntity | null = await this.repository.findOne({
      where: {
        id,
      },
      select: columns,
    });
    if (candidate === null) {
      throw new UserNotFoundException("id", id);
    }
    if (newest.password) {
      newest.password = await this.passwordService.hash(newest.password);
    }
    candidate = this.repository.merge(candidate, newest);
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
  ): Promise<GetUserDto | null> {
    return await this.repository.findOneBy(findOptions);
  }

  private getColumnsName(): (keyof UserEntity)[] {
    return this.repository.metadata.columns.map(
      (col: ColumnMetadata): string => col.propertyName,
    ) as (keyof UserEntity)[];
  }
}
